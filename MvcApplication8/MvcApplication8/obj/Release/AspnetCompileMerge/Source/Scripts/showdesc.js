$(document).ready(function(){
    $(".desc").click(function () {
        var thisobject = $(this);
        $(".hidden").each(function(){

            if($(this). css("display") != "none") 
                $(this).css("display", "none");
        });
        
        thisobject.parent().next().slideDown(600);
                   
    });
    $( ".desc" ).hover(function() {
        $(this).css({ "background-color": "#c7d1d6", "cursor": "pointer" });
    });
    $(".desc").mouseout(function () {
        $(this).css("background-color", "#efeeef");
    });
    
    $(".voteimage").hover(function () {
        $(this).css({ "opacity": "1", "cursor": "pointer" });
    });
    $(".voteimage").mouseout(function () {
        $(this).css("opacity", "0.4");;
    });
});


    