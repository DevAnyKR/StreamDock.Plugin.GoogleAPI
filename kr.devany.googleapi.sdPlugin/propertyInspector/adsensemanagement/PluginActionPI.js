document.addEventListener('load', function () {
	setTimeout(() => {
		setSettings();
	}, 100);
});

document.addEventListener('websocketCreate', function () {
	setTimeout(() => {
		ResourceTypeChanged();
	}, 10);
});

function ResourceTypeChanged(){
	let Selected = $("#resource").find("option:selected");

	switch(Selected.val()){
		case "Payments":
			$("#dvDateRange").hide();
			$("#dvMetrics").hide();
			$("#dvDimensions").hide();
			break;
		case "Reports":
			$("#dvDateRange").show();
			$("#dvMetrics").show();
			$("#dvDimensions").hide();
			break;
		case "Dimensions":
			$("#dvDateRange").show();
			$("#dvMetrics").show();
			$("#dvDimensions").show();
			break;
	}
}