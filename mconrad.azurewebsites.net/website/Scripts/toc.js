(function () {   
    var wrapper = $("<div/>");
    var tocDiv = $(".toc");
    wrapper.append(tocDiv);

    // add nav class to top and sub ul elements
    $("ul", wrapper).addClass("nav")
    .data("spy", "affix").data("offset-top", "195");

    // unwrap the main ul
    toc = $("ul", wrapper).first().unwrap();
    toc.addClass("hidden-print hidden-xs hidden-sm");

    $("#tocNav").append(toc);
        
    $("body").scrollspy({ target: '#tocNav' });

    $(toc).affix({
        offset: {
            top: function () {
                return -1;
            }
        }
    });
    $("body").scrollspy("refresh");

    // images defined by md are not responsive, lets make them responsive
    $("img").addClass("img-responsive");

    $("#docBody table").addClass("table table-striped table-bordered");
})();