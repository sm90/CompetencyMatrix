
function reorderEvaluationModelLevelsQuality(event, ui)
{
	var $modelDiv = ui.item.closest("[skillEvaluationModelId]");

	var $listOfLevels = $(".listOfLevels", $modelDiv);
	var $levelRows = $(".levelRow", $listOfLevels);

	$levelRows.each(function(index)
	{
		var $row = $(this);

		var $hiddenQuality = $(".LevelQuality", $row);
		$hiddenQuality.val(index + 1);

		var $testDisplayQuality = $(".LevelQualityTest", $row);
		$testDisplayQuality.html(index + 1);
	});

	updateSkillEvaluationModelsEditorListBox();
}

function bindSkillEvaluationModelEditorEvents(modelId)
{
	var $modelDiv = $("[skillEvaluationModelId=\"" + modelId + "\"]");

	$(".listOfLevels", $modelDiv).sortable(
		{
			axis: "y",
			containment: "parent",
			stop: reorderEvaluationModelLevelsQuality
		});
	$(".listOfLevels", $modelDiv).disableSelection();

	//Update select list item when you change name
	$(".EvaluationModelName", $modelDiv).keyup(function ()
	{
		updateSkillEvaluationModelsEditorListBox();
	});
	
	$(".btnAddEvaluationModelLevel", $modelDiv)
	.bind("click",
		function (event, data)
		{
			var $btn = $(this);
			var $modelDiv = $btn.closest("[skillEvaluationModelId]");
			var hiddenIndexValue = $("#Index", $modelDiv).val();
			var $list = $btn.closest(".listOfLevels", $modelDiv);
			var $closestLevelRow = $btn.closest(".levelRow", $list);
			
			var url = window._SkillEvaluationModel_AddLevel;
			
			if ($closestLevelRow.selectorExists())
			{
				var currentRowId = $(".LevelId", $closestLevelRow).val();
				$(".currentLevelId", $modelDiv).val(currentRowId);

				var currentRowQuality = $(".LevelQuality", $closestLevelRow).val();
				$(".currentLevelQuality", $modelDiv).val(currentRowQuality);
			} else
			{
				$(".currentLevelId", $modelDiv).val(0);
				$(".currentLevelQuality", $modelDiv).val(0);
			}

			var modelDataArray = $("input", $modelDiv).serializeArray();
			var removedPrefix = "[" + hiddenIndexValue + "].";

			modelDataArray.forEach(function(obj, index, arr)
			{
				obj.name = obj.name.replace(removedPrefix, "");
			});

			var modelDataFormEncoded = $.param(modelDataArray);
			var modelData = modelDataFormEncoded;

			$.ajax({
				url: url,
				type: "POST",
				data: modelData,
				success: function (result, status)
				{
					if (status !== "error")
					{
						$modelDiv.replaceWith(result);
						updateSkillEvaluationModelsEditorListBox();
					}
				}
			});
		});
}

