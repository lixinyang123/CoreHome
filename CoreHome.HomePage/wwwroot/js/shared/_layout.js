//===================Adapter=======================
var isMobile = {
	Android: function () {
		return navigator.userAgent.match(/Android/i);
	},
	iOS: function () {
		return navigator.userAgent.match(/iPhone|iPad|iPod/i);
	},
	Windows: function () {
		return navigator.userAgent.match(/IEMobile/i);
	},
	any: function () {
		return (isMobile.Android() || isMobile.iOS() || isMobile.Windows());
	}
};

function fullHeight() {

	if (!isMobile.any()) {
		var list = document.getElementsByClassName("js-fullheight");

		for (var i = 0; i < list.length; i++) {
			list[i].style.height = window.innerHeight + "px";
		}
	}
}

function MoveTop()
{
	$("html,body").animate({ scrollTop: 0 }, 500);
}

//=============GetVerfyCode=============

function getVerfyCode(img) {
	img.src = "/Service/VerificationCode?" + Math.random();
}

//=============Initializa================

function init() {
	window.onscroll = () => {
		if (window.scrollY > 200) {
			document.querySelector(".js-top").classList.add("active");
		}
		else {
			document.querySelector(".js-top").classList.remove("active");
        }
    }
	window.onresize = fullHeight;
	fullHeight();
	ScrollReveal().reveal(".reveal");
}

init();
