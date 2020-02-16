//===================Adapter=======================
var isMobile = {
	Android: function () {
		return navigator.userAgent.match(/Android/i);
	},
	BlackBerry: function () {
		return navigator.userAgent.match(/BlackBerry/i);
	},
	iOS: function () {
		return navigator.userAgent.match(/iPhone|iPad|iPod/i);
	},
	Opera: function () {
		return navigator.userAgent.match(/Opera Mini/i);
	},
	Windows: function () {
		return navigator.userAgent.match(/IEMobile/i);
	},
	any: function () {
		return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
	}
};

function fullHeight() {

	if (!isMobile.any()) {
		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function () {
			$('.js-fullheight').css('height', $(window).height());
		});
	}
}

fullHeight();

function MoveTop()
{
	var current = document.scrollingElement.scrollTop;
	current-=30;
	document.scrollingElement.scrollTop = current;
	timer = setTimeout(MoveTop,0.1);
	if(document.scrollingElement.scrollTop==0)
	{
		clearTimeout(timer);
	}
}

$(window).scroll(function () {

    var $win = $(window);
    if ($win.scrollTop() > 200) {
        $('.js-top').addClass('active');
    } else {
        $('.js-top').removeClass('active');
    }
});

//GetVerfyCode
function getVerfyCode(id) {
    document.getElementById(id).src = "/Home/VerificationCode?" + Math.random();
}