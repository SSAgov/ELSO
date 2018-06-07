/* 
* Accesskey JavaScript Document 
* created by:  [PERSON_NAME]
* last modified: 6/10/10 
*/

/*************************/
/*   ACCESSKEY FUNCTIONS      */
/*********************** */
$(function () {
    /* Establish Array to store values */
    var $accesskeyToolTip = [];

    /* Iterate through Accesskeys on page */
    $("input[accesskey],button[accesskey],select[accesskey],textarea[accesskey]").each(function (index) {
        $accesskeyToolTip[index] = $(this).attr("accesskey");
        switch ($(this).css("float")) {
            case "left":
                $(this).before("<span class='tooltipLeft'>" + $accesskeyToolTip[index] + "</span>");
                break;
            case "right":
                $(this).before("<span class='tooltipRight'>" + $accesskeyToolTip[index] + "</span>");
                break;
            default:
                $(this).before("<span class='tooltip'>" + $accesskeyToolTip[index] + "</span>");
                break;
        }
    });

    /* When Ctrl key is pressed show acceskey tooltip | otherwise hide */
    $(document).keydown(function (e) {
        if (e.ctrlKey) {
            if ($("div[disabled]").attr("disabled") != "true") {
                $("span[class^='tooltip']").show();
            }
        }
    }).keyup(function () {
        $("span[class^='tooltip']").hide();
    });

});