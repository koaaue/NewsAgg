
var validateField = function (fieldElem, infoMessage, validateFn) {
    // TODO: Implement validateField.

    fieldElem.next().removeClass();

    if (fieldElem.val() == "") {

        fieldElem.next().text(infoMessage);             //先创建好span标签，然后.next().text()修改内容
        fieldElem.next().addClass("info");				//.next().addClass()修改css样式
    }
};


$(document).ready(function () {
    // TODO: Use validateField to validate form fields on the page.

    //var info;

    /*focus进入焦点时触发；blur失去焦点时触发*/
    $(".desc").click(function () {                                  //click
        $(".hidden").each(function () {                             //效果比blur好
            if ($(this).css("display") != "none")
                $(this).css("display", "none");
        });

        $(this).parent().next().css("display", "inline");          //display很乱
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
