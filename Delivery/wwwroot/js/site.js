﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$().ready(function (){
    $('#button-addon1').click(function (e){
        e.preventDefault();
        if ($('#pass').attr('type')==='password'){
            $('#pass').attr('type', 'text');
        }
        else {
            $('#pass').attr('type', 'password');
        }
    });
    $('#button-addon2').click(function (e){
        e.preventDefault();
        if ($('#confPass').attr('type')==='password'){
            $('#confPass').attr('type', 'text');
        }
        else {
            $('#confPass').attr('type', 'password');
        }
    });
    
});