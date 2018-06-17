import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable()
export class AlertityService {

constructor() { }

confirm(message: string, okCollback: () => any ) {
    alertify.confirm(message, function(e){
        if (e) {
            okCollback();
        } else {

        }
    });
}

succes(message: string) {
    alertify.success(message);
}

error(message: string) {
    alertify.error(message);
}

warning(message: string) {
    alertify.warning(message);
}

message(message: string) {
    alertify.message(message);
}

}