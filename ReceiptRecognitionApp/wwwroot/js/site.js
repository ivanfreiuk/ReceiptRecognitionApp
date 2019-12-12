﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    // input plugin
    bsCustomFileInput.init();

    // get file and preview image
    $("#file").on('change', function () {
        var input = $(this)[0];
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#preview').attr('src', e.target.result).fadeIn('slow');
            }
            reader.readAsDataURL(input.files[0]);
        }
    });

    var myJSON = $('#json').text().replace(/\,/g, ',<br />').replace(/\{/g, '{<br />').replace(/\}/g, '<br />}');
    document.getElementById("showjson").innerHTML = myJSON;
});

$('.zoom-btn').click(function () {
    $('#myModel').empty();
    $('#myModel').append($(this).children('img').eq(0).clone())
    $('#myModel img').css('height', 'auto');
});

