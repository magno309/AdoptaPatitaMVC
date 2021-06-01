$(document).ready(function(){
    
    $('#registro-form').bootstrapValidator({
        fields:{
            nameRef: {
                validators:{
                    notEmpty:{
                        message:'Favor de introducir el nombre de su refugio'
                    },
                    stringLength:{
                        message: 'Longitud máxima de 20.',
                        max: 20 
                    }
                }
            },
            nameRep:{
                validators:{
                    notEmpty:{
                        message:'Favor de introducir el nombre del representante'
                    },
                    stringLength:{
                        message: 'Longitud máxima de 100 y mínima de 5.',
                        min: 5,
                        max: 100
                    }   
                }
            },
            txtEmail:{
                validators:{
                    notEmpty:{
                        message:'Favor de introducir un email de contacto'
                    },
                    regexp:{
                        regexp: "^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$",
                        message: 'Introduzca un email válido.'
                    }
                }
            },
            txtPass:{
                validators:{
                    notEmpty:{
                        message:'Ingrese una contraseña'
                    },
                    regexp:{
                        regexp: "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})",
                        message: 'Contraseña debe contener al menos: 1 digito, 1 mayuscula, 1 caracter especial, 8 caracteres'
                    }
                }
            },
            txtCiudad:{
                validators:{
                    notEmpty:{
                        message:'Ingrese la ciudad donde se localiza su refugio. '
                    },
                    stringLength:{
                        message: 'Máximo 30 caracteres.',
                        max:30
                    }
                }                
            },
            txtCalle:{
                validators:{
                    notEmpty:{
                        message:'Ingrese la calle donde se localiza. '
                    },
                }
            },
            txtNumero:{
                validators:{
                    notEmpty:{
                        message:'Ingrese el número de ubicación. '
                    },
                }  
            },
            txtTel:{
                validators:{
                    notEmpty:{
                        message:'Teléfono de contacto. '
                    },
                    regexp:{
                        regexp: "^[0-9]+$",
                        message: 'Ingrese solo números'
                    },
                    stringLength:{
                        message: 'Mínimo 10 dígitos ',
                        min:10
                    }
                    
                }               
            },
            txtNumPeludos:{
                validators:{
                    notEmpty:{
                        message:'Ingrese la cantidad de animales que hay en su refugio '
                    },
                    between:{
                        message:' Para considerarse un refugio, usted debe tener más animales. ',
                        min:5,
                        max:1000000
                    }
                }
            },
            txtHistoria:{
                validators:{
                    notEmpty:{
                        message:'Ingrese una breve historia del origen de su refugio. '
                    },
                    stringLength:{
                        message: 'Máximo 1000 caracteres. ',
                        max: 1000
                    }
                }
            },
            txtPasos:{
                validators:{
                    notEmpty:{
                        message:'Favor de introducir claramente los pasos a seguir. '
                    }
                }
                
            }
        }
    });
    var btnEnviar = document.getElementById("btnEnviar");
    btnEnviar.onclick = clickEnviar;
});



var token2 = $('input:hidden[name="__RequestVerificationToken"]').val();
function clickEnviar(){
    $('#registro-form').data('bootstrapValidator').validate();
    if($('#registro-form').data('bootstrapValidator').isValid()){
        var xhttp = new XMLHttpRequest();
        uri = "../Identity/Account/Register?handler=Async";        
        xhttp.open("POST", uri, true);
        xhttp.setRequestHeader("RequestVerificationToken", token2);
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
                if (validaRegistro()){
                    window.location.href="SolicitudEnviada";
                } else {
                    window.location.href="ErrorSolicitud";
                }
            }
        };
        var diccionario = new FormData();
        diccionario.append("Input.Email",  document.getElementById("txtEmail").value);
        diccionario.append("Input.Password", document.getElementById("txtPass").value );
        diccionario.append("Input.ConfirmPassword",  document.getElementById("txtPass").value);
        diccionario.append("__RequestVerificationToken", token2);
        diccionario.append("Refugio", "TRUE");
        diccionario.append("Nombre", document.getElementById("nameRef").value);
        diccionario.append("Direccion", document.getElementById("txtCalle").value + " #" + 
                                        document.getElementById("txtNumero").value+ ",  "+
                                        document.getElementById("txtCiudad").value);
        diccionario.append("Telefono", document.getElementById("txtTel").value);
        diccionario.append("Email", document.getElementById("txtEmail").value);
        diccionario.append("Contrasenia", "-");
        diccionario.append("Sitio_web", "-");
        xhttp.send(diccionario);
          
        
        
    }
}

/*
,
                stringLength:{
                    message: 'Longitud máxima de 20.',
                    max: 20 
                }


$(document).ready( function (){
    $('#registro-form').validate({
        rules:{
            nameRef: {
                required: true,
                maxlength: 20 
            },
            nameRep:{
                required: true,
                maxlength: 100,
                minlength: 5    
            },
            txtEmail:{
                required: true,
                miEmail: true,
            },
            txtPass:{
                required: true,
                miPass: true
            },
            txtCiudad:{
                required: true,
                maxlength:30
            },
            txtCalle:{
                required: true
            },
            txtNumero:{
                required: true
            },
            txtTel:{
                required: true,
                miTel:true,
                minlength: 10
            },
            txtNumPeludos:{
                required: true,
                min: 5
            },
            txtHistoria:{
                required: true,
                maxlength: 1000
            },
            txtPasos:{
                required: true
            }
        },
        messages:{
            nameRef: {
                required: '<div class="invalid-feedback" style="display:block;">Favor de introducir un el nombre de su refugio.</div>',
                maxlength: '<div class="invalid-feedback" style="display:block;">Máximo 20 caracteres.</div>' 
            },
            nameRep:{
                required: '<div class="invalid-feedback" style="display:block;">Favor de introducir el nombre del representante.</div>',
                maxlength: '<div class="invalid-feedback" style="display:block;">Longitud máxima de 100.</div>',
                minlength: '<div class="invalid-feedback" style="display:block;">Longitud mínima de 5</div>'    
            },
            txtEmail:{
                required: '<div class="invalid-feedback" style="display:block;">Favor de introducir un email de contacto</div>',
                miEmail: '<div class="invalid-feedback" style="display:block;">Introduzca un email válido.</div>',
            },
            txtPass:{
                required: '<div class="invalid-feedback" style="display:block;">Ingrese una contraseña.</div>',
                miPass: '<div class="invalid-feedback" style="display:block;">Contraseña debe contener al menos: 1 digito, 1 mayuscula, 1 caracter especial, 8 caracteres</div>'
            },
            txtCiudad:{
                required: '<div class="invalid-feedback" style="display:block;"> Ingrese la ciudad donde se localiza su refugio. </div>',
                maxlength:'<div class="invalid-feedback" style="display:block;"> Máximo 30 caracteres. </div>'
            },
            txtCalle:{
                required: '<div class="invalid-feedback" style="display:block;"> Ingrese la calle donde se localiza. </div>'
            },
            txtNumero:{
                required: '<div class="invalid-feedback" style="display:block;"> Ingrese el número de ubicación. </div>'
            },
            txtTel:{
                required: '<div class="invalid-feedback" style="display:block;"> Teléfono de contacto. </div>',
                miTel: '<div class="invalid-feedback" style="display:block;"> Ingrese solo números </div>',
                minlength: '<div class="invalid-feedback" style="display:block;"> Mínimo 10 dígitos </div>'
            },
            txtNumPeludos:{
                required: '<div class="invalid-feedback" style="display:block;"> Ingrese la cantidad de animales que hay en su refugio </div>',
                min: '<div class="invalid-feedback" style="display:block;"> Para considerarse un refugio, usted debe tener más animales. </div>'
            },
            txtHistoria:{
                required: '<div class="invalid-feedback" style="display:block;"> Ingrese una breve historia del origen de su refugio. </div>',
                maxlength: '<div class="invalid-feedback" style="display:block;"> Máximo 1000 caracteres. </div>'
            },
            txtPasos:{
                required: '<div class="invalid-feedback" style="display:block;"> Favor de introducir claramente los pasos a seguir. </div>'
            }
        }
    });

    // Método de validacion de caracteres de la contraseña
    jQuery.validator.addMethod("miPass", function(value, element) {
        let regExp = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})";
        return this.optional(element) || value.match(regExp);
      });

      // Validación de email
    jQuery.validator.addMethod("miEmail", function(value, element) {
        let regExp = "^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        return this.optional(element) || value.match(regExp);
        
    });

    // Validación de telefono solo numeros
    jQuery.validator.addMethod("miTel", function(value, element) {
        let regExp = "^[0-9]+$";
        return this.optional(element) || value.match(regExp);
        
    });
    


     
   
});*/