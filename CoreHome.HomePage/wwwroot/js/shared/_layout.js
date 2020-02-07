
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