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
		var list = document.getElementsByClassName("js-fullheight");

		for (var i = 0; i < list.length; i++) {
			list[i].style.height = window.innerHeight + "px";
		}
	}
}

function MoveTop()
{
	timer = setInterval(function () {
		var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
		var speed = Math.floor(-scrollTop / 8);
		document.documentElement.scrollTop = document.body.scrollTop = scrollTop + speed;
		isTop = true; 
		if (scrollTop == 0) {
			clearInterval(timer);
		}
	}, 50);
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
