function comprobarContrasenia() {
    contra = document.getElementById("contra").value;
    confirmar_contra = document.getElementById("confirmar_contra").value;
    const FailContra = document.getElementById("FailContra");
    if (contra != confirmar_contra) {
        FailContra.innerHTML = '<p class="my-2 text-center text-danger">Las contraseñas son diferentes</p>';
    }
    else {
        FailContra.innerHTML = '<p class="hide my-0 text-center text-danger"></p>';
    }
}