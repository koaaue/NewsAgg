﻿
$(document).ready(function () {

    //$(".hidden").each().hide();

    $(".desc").click(function () {                                  //click
        $(".hidden").each(function () {                             //效果比blur好
            if ($(this).css("display") != "none")
                $(this).css("display", "none");
        });

        $(this).parent().next().css("display", "");          //display很乱, 手机display乱
        
    });

    /*$(".desc").blur(function () {
        $(this).parent().next().css("display", "none");
    });*/
    $(".desc").hover(function () {
        $(this).css({ "background-color": "#c7d1d6", "cursor": "pointer" });
    });
    $(".desc").mouseleave(function () {                             //比mouseout好
        $(this).css("background-color", "#efeeef");
    });
});


function splitKeyword( str ) {             //需要传参数

    //String [] word = str.split(',');
    $("#" + obj).text("<>");
}