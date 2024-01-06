
function mask(o, f) {
    setTimeout(function () {
        var v = mphone(o.value);
        if (v != o.value) {
            o.value = v;
        }
    }, 1);
}

function mphone(v) {
    var r = v.replace(/\D/g, "");
    r = r.replace(/^0/, "");
    if (r.length > 10) {
        r = r.replace(/^(\d\d)(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else if (r.length > 5) {
        r = r.replace(/^(\d\d)(\d{4})(\d{0,4}).*/, "($1) $2-$3");
    } else if (r.length > 2) {
        r = r.replace(/^(\d\d)(\d{0,5})/, "($1) $2");
    } else {
        r = r.replace(/^(\d*)/, "($1");
    }
    return r;
}

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function verificaForcaSenha(campo,mensagem) {
    var passwordTextBox = document.getElementById(campo);
    var password = passwordTextBox.value;
    var caracteresEspeciais = "!£$%^&*_@#~?";
    var passwordPontos = 0;
    var codForca = 0;

    // Contém caracteres especiais
    for (var i = 0; i < password.length; i++) {
        if (caracteresEspeciais.indexOf(password.charAt(i)) > -1) {
            passwordPontos += 20;
            break;
        }
    }
    // Contém numeros
    if (/\d/.test(password))
        passwordPontos += 20;
    // Contém letras minúsculas
    if (/[a-z]/.test(password))
        passwordPontos += 20;
    // Contém letras maiúsculas
    if (/[A-Z]/.test(password))
        passwordPontos += 20;
    if (password.length >= 8)
        passwordPontos += 20;
    var forcaSenha = "";
    var backgroundColor = "red";
    if (passwordPontos >= 100) {
        forcaSenha = "Forte";
        backgroundColor = "green";
        codForca = 3;
    }
    else if (passwordPontos >= 80) {
        forcaSenha = "Média";
        backgroundColor = "gray";
        codForca = 2;
    }
    else if (passwordPontos >= 60) {
        forcaSenha = "Fraca";
        backgroundColor = "maroon";
        codForca = 1;
    }
    else {
        forcaSenha = "Muito Fraca";
        backgroundColor = "red";
        codForca = 0;
    }

    document.getElementById(mensagem).innerHTML = forcaSenha;
    return codForca;
}

function validarConfirmacaoSenha(senha, confirmarSenha, statusConfirmarSenha) {
    var codForca = 0;

    if (document.getElementById(senha).value !== document.getElementById(confirmarSenha).value) {
        document.getElementById(statusConfirmarSenha).innerHTML = 'Senhas não correspondem';
        codForca = 0;
    } else {
        document.getElementById(statusConfirmarSenha).innerHTML = '';
        codForca = 1;
    }

    return codForca;
}



