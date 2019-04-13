// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ---------------------------------------------
// javascript file site.js
// ---------------------------------------------

function ComboBoxItemOnMouseOut(t) {
    t.style.backgroundColor = '#FFFFFF';
}
function ComboBoxItemOnMouseOver(t) {
    t.style.backgroundColor = '#D3D3D3';
}
function ComboBoxItemClick(t, textBoxDisplayID) {

    var v = $(t).attr('data-value');
    elid = "#" + textBoxDisplayID;
    $(elid).val(v);


}


// -------------------------------------------------------------
// Displays the wait cursor
// -------------------------------------------------------------
function OnReadFeedSubmit() {

    var rslt = false;

    var url = $('#COMBOBOX_FEED_URL').val();
    url = url.trim();
    if (url.length > 0) {
        document.body.style.cursor = 'wait';
        $('#waitcursordiv').show();
        rslt = true;
    }
    else {
        alert("Error In RSS Feed URL");
    }
    return rslt;
}


// -------------------------------------------------------------
// Displays the wait cursor
// -------------------------------------------------------------
function OnCreateFeedSubmit() {

    var rslt = false;

    var testID = $('#TEST_ID').val();
    alert("TESTID: " + testID.toString());

    return rslt;
}


function ReturnHome() {
    window.location.href = "/Home/Index";
}


// -----------------------------------------------------------
// Oncreatefeed test select
// -----------------------------------------------------------
function OnCreateFeedTestSelect(t) {

    var txt = $(t).attr('data-value');
    $('#TEST_DESCRIPTION').html(txt);

}

