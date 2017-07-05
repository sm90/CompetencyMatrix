function refreshEvaluationModelSelect()
{
	var $modelsListBox = $("#skillEvaluationModels");

	if ($modelsListBox.selectorExists())
	{
		var $selectedOption = $("option:selected", $modelsListBox);
		var selectedValue = $selectedOption.val();

		var url = window._SkillEditor_GetEvaluationModelsOptions;

		$.ajax({
			url: url,
			type: "GET",
			data: { selectedId: selectedValue },
			success: function (result, status)
			{
				if (status !== "error")
				{
					$modelsListBox.html(result);
				}
			}
		});
	}
}