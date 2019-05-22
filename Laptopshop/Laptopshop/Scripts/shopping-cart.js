$(function () {

    $(".gw-draggable").draggable({ revert: "valid" }, { stack: "div" });

    $(".gw-droppable").droppable({
        drop: function (event, ui) {
            var id = $(ui.draggable[0]).attr("data-product-id");

            $.ajax({
                url: "/Cart/Add",
                data: { id: id },
                success: function (response) {
                    $("#cart-cnt").html(response.Count);
                    $("#cart-amt").html(response.Amount);

                    $("#cart-img").stop().effect("bounce");
                }
            });


        }
    });

    $("[data-add-to-cart]").click(function () {

        var id = $(this).attr("data-add-to-cart");

        $.ajax({
            url: CART_ADD,
            data: { id: id },
            success: function (response) {
                $("#cart-cnt").html(response.Count);
                $("#cart-amt").html(response.Amount);
            }
        });

        var img = $(this).parents(".panel").find(".panel-body img");
        var css = ".cart-fly {background-image: url('" + img.attr("src") + "');background-size: 100% 100%;}";
        $("#cart-fly").html(css);
        img.effect("transfer", { to: "#cart-img", className: "cart-fly" }, 1000, function () {
            $("#cart-img").stop().effect("bounce");
        });
    });

    //Remove
    $("[data-remove-from-cart]").click(function () {
        var id = $(this).attr("data-remove-from-cart");
        $.ajax({
            url: CART_REMOVE,
            data: { id: id },
            success: function (response) {
                $("#cart-cnt").html(response.Count);
                $("#cart-amt").html(response.Amount);
                $("#table-cart-amt").text(Math.round(response.Amount));
            }
        });
        //Xóa một dòng trên giao diện
        $(this).parents("tr").hide(500);
    });
    //Update
    $("[data-update-cart]").change(function () {
        var id = $(this).attr("data-update-cart");
        var qty = $(this).val();
        console.log('client: ' + qty);
        $.ajax({
            url: CART_UPDATE,
            data: { id: id, newqty: qty },
            success: function (response) {
                $("#cart-cnt").html(response.Count);
                $("#cart-amt").html(response.Amount);
                $("#table-cart-amt").text(Math.round(response.Amount));
                console.log('server: ' + response.Count);
            }
        });

        //Cập nhật trên giao diện
        var price = parseFloat($(this).parents("tr").find("td:eq(2)").text());
        var discount = parseFloat($(this).parents("tr").find("td:eq(3)").text());
        var amount = Math.round(price * qty * (1 - discount));
        $(this).parents("tr").find("td:eq(5)").text(amount);
     

    });
});