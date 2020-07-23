jQuery(document).ready(function () {
	$('.progress-bar').each(function () {
		var percent = $(this).attr('data-percentage'),
			skillLevel = "";

		getPerAsNum = percent.split("%");
		if (getPerAsNum[0] == 33 || getPerAsNum[0] < 33) {
			skillLevel = "Beginner";
		}
		if (getPerAsNum[0] == 34 || getPerAsNum[0] == 66 || getPerAsNum[0] > 34 && getPerAsNum[0] < 66) {
			skillLevel = "Intermediate";
		}
		if (getPerAsNum[0] == 100 || getPerAsNum[0] > 66) {
			skillLevel = "Advanced";
		}

		$(this).find('.progress-content').animate({
			width: percent
		}, 2000);

		$(this).find('.progress-number-mark').animate(
			{ left: percent },
			{
				duration: 2000,
				step: function (now, fx) {
					var data = Math.round(now);
					$(this).find('.percent').html(skillLevel);
				}
			});
	});
});