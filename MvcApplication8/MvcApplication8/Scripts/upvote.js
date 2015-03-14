function ClickUpvote(obj) {
    //alert("alert test.");

    var Li = {
        UserName: "",
        ItemId: obj
    }

   



   $.ajax({
        type: "POST",
        url: "/Xml2Model/Index",
        data: Li,
        success: function (x) {
            if (x==1) {
                document.getElementById(obj).innerHTML = parseInt(document.getElementById(obj).innerHTML) + 1;     
                
            }
            else if (x == 0) {
                //window.location.href = "http://koaaue-001-site1.myasp.net/Account/Login/";
                window.location.href = "/Account/Login/";
            }
            else {
                alert('You have already voted');
            }

        }});
    

    

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
            url: "/Xml2Model/Index",
            data: Li/*,
            success: function (data) {
            }*/
        });
    });

});

