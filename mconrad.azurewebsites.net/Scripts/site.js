(function () {
    /* making navbar pop out on hover */
    $(".navbar-nav>li.dropdown").hover(
        function (elem) {
            $(this).addClass("open")
        },
        function (elem) {
            $(this).removeClass("open")
        }
    );
    
    // back to top button
    var offset = 250;
    var duration = 300;
    $('.back-to-top').hide();
    $(window).scroll(function () {
        if ($(this).scrollTop() > offset) {
            $('.back-to-top').fadeIn(duration);
        } else {
            $('.back-to-top').fadeOut(duration);
        }
    });

    $('.back-to-top').click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    })

    // google code prettify
    $("pre>code").addClass("prettyprint");
    prettyPrint();
})();
