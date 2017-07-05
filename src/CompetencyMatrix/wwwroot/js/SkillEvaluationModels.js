function skillEvaluationModelsEditor_success() {
    $("#skillEvaluationModelsEditDialog").modal("hide");
}

function updateSkillEvaluationModelsEditorListBox() {
    var $modelsListBox = $("#skillEvaluationModelsList");

    if ($modelsListBox.selectorExists()) {
        var $selectedOption = $("option:selected", $modelsListBox);
        var modelId = $selectedOption.val();

        var $modelDiv = $("[skillEvaluationModelId=\"" + modelId + "\"]");

        var modelName = $(".EvaluationModelName", $modelDiv).val();

        var $listOfLevels = $(".listOfLevels", $modelDiv);
        var $levelItems = $(".levelRow", $listOfLevels);
        var levelNames = [];

        $levelItems.each(function (index) {
            var $item = $(this);

            //var hiddenLevelName = $(".LevelName", $item).text();
            var hiddenLevelName = $(".LevelHiddenName", $item).val();
            levelNames.push(hiddenLevelName);
        });

        var modelDisplayName = modelName;

        if (levelNames.length === 0) {
            modelDisplayName += " [no levels]";
        }
        else {
            modelDisplayName += " [" + levelNames.join(", ") + "]";
        }

        $selectedOption.text(modelDisplayName);
    }
}

function changeModel() {
    var modelId = $("#skillEvaluationModelsList").val();

    if (modelId != null) {

        var $modelDivToShow = $("[skillEvaluationModelId=\"" + modelId + "\"]");
        var $modelDivsToHide = $("[skillEvaluationModelId][skillEvaluationModelId!=\"" + modelId + "\"]");

        $("#divNoModels").css("display", "none");
        $modelDivToShow.css("display", "block");
        $modelDivsToHide.css("display", "none");
    }
    else {
        $("#divNoModels").css("display", "block");
        $("[skillEvaluationModelId]").css("display", "none");
    }

    if (modelId && (modelId.length > 1 || modelId.length === 0)) {
        $("#btnDeleteEvaluationModel").addClass('disabled')
    }
    else {
        $("#btnDeleteEvaluationModel").removeClass('disabled')
    }
}

function bindSkillEvaluationsModelEditorEvents() {
    //Wire on change for listbox
    $("#skillEvaluationModelsList")
		.change(changeModel);

    $("#btnAddEvaluationModel")
		.click(
			function () {
			    var url = window._SkillEvaluationModels_Add;

			    var data = $("#skillEvaluationModelsEditorForm").serialize();

			    $.ajax({
			        url: url,
			        type: "POST",
			        data: data,
			        success: function (result, status) {
			            if (status !== "error") {
			                $("#skillEvaluationModelsEditor").replaceWith(result);
			                changeModel();

			                //var optionNewModel = new Option("New model *", -1, true, true);

			                //$("#skillEvaluationModelsList option").removeAttr("selected");
			                //$("#skillEvaluationModelsList").append($(optionNewModel));

			                //$("#skillEvaluationModelsCurrentModel").css("display", "block");
			                //$("#divNoModels").css("display", "none");
			            }
			        }
			    });
			}
		);

    $("#btnDeleteEvaluationModel")
		.click(
			function () {

			    var modelId = $("#skillEvaluationModelsList").val();
			    if (modelId != null) {

			        var url = _Skills_GetByModel;

			        var data =
                        {
                            modelId: modelId[0]
                        };

			        $.ajax({
			            url: url,
			            type: "GET",
			            data: data,
			            success: function (result, status)
			            {
			                if (result.length > 0) {
			                    var message = 'This model is used in the following skills definition: {0} You will lost the levels definitions if this model is removed. Do you want to proceed?',
			                        skills = '';

			                    for (var i = 0; i < result.length; i++) {
			                        skills += result[i].name + (i === result.length - 1 ? '. ' : ', ');
			                    }
			                    
			                    showConfirm('Confirm', String.format(message, skills), function () {
			                        deleteModel(modelId);
			                    });

			                }
			                else {
			                    deleteModel(modelId);
			                }
			            }
			        });
			    }
			}
		);

    function deleteModel(modelId)
    {
        var $modelDiv = $("[skillEvaluationModelId=\"" + modelId + "\"]");

        $("#skillEvaluationModelsList option:selected").remove();
        $modelDiv.remove();

        changeModel();
    }
}
