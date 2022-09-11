$(document).ready(main);




$.get("https://ipapi.co/json/", function (data) {
    console.log("get success");
})
  .done(function (res) {
      setCookie('ip', res.ip, 1);
      setCookie('country_code', res.country_code, 1);
      console.log("done success");
  })
  .fail(function (err) {
      setCookie('ip', '', 1);
      setCookie('country_code', 'SY', 1);
      console.log("fail error");
  });
function setCookie(name, value, hours) {
    var expires = "";
    if (hours) {
        var date = new Date();
        date.setTime(date.getTime() + ( hours * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function main () {
    
    var usernameTextBox = $('#username_textbox');
    var passwordTextBox = $('#password_textbox');
    
    usernameTextBox.keypress(keypress_handler);
    passwordTextBox.keypress(keypress_handler);
    
    usernameTextBox.blur(blur_handler);
    passwordTextBox.blur(blur_handler);
}


function keypress_handler (e) {
    $(this).addClass('inputted-text');
}


function blur_handler () {
    if ($(this).val().length == 0)
        $(this).removeClass ('inputted-text');
}

