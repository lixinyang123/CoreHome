//===================Adapter=======================
let isMobile = {
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
	if (isMobile.any()) return;

	document.querySelectorAll(".js-fullheight").forEach(item => {
		if (!document.startViewTransition) {
			item.style.height = `${window.innerHeight}px`;
			return;
		}

		document.startViewTransition(() => {
			item.style.height = `${window.innerHeight}px`;
		});
	});
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
	window.onresize = fullHeight;
	window.onscroll = () => {
		if (window.scrollY > 200) {
			document.querySelector(".js-top").classList.add("active");
		}
		else {
			document.querySelector(".js-top").classList.remove("active");
        }
    }
	ScrollReveal().reveal(".reveal");
	fullHeight();
}

init();
