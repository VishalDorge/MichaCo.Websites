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
})();
