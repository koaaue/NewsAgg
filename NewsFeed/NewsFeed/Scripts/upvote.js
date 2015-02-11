function ClickUpvote(obj) {
    //alert("alert test.");

    var Li = {
        UserName: "",
        ItemId: obj
    }
    $.ajax({
        type: "POST",
        url: "/News/Index",
        data: Li/*,
            success: function (data) {
            }*/
    });


    document.getElementById(obj).innerHTML = "+1";     //只能改变第一行啊


}

//===AJAX, 后台访问url并修改数据库===
$(document).ready(function () {


    $("#upvote-ajax").on("click", function () {
        var Li = {
            UserName: "ab",
            ItemId: 13
        }
        $.ajax({
            type: "POST",
            url: "/News/Index",
            data: Li/*,
            success: function (data) {
            }*/
        });
    });

});

