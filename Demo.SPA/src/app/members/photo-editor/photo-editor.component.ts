import { AlertityService } from './../../_services/alertity.service';
import { error } from 'selenium-webdriver';
import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';
import { environment } from './../../../environments/environment';
import { Photo } from './../../_models/Photo';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import * as _ from 'underscore';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
  @Output() getMemberPhotoChange = new EventEmitter<string>();

  constructor(private authService: AuthService, private userService: UserService,
    private alertityService: AlertityService) { }

  ngOnInit() {
    this.initializerUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializerUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodeToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.authService.changeMemberPhoto(photo.url);
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };
  }

  deletePhoto(id: number) {
    this.alertityService.confirm('Are you sure you want delete this photo', () => {
      this.userService.deletePhoto(this.authService.decodeToken.nameid, id).subscribe(() => {
        this.photos.splice(_.findIndex(this.photos, { id: id }), 1);
      }, error => {
        this.alertityService.error('W cant delete photo');
      });
  });

  }


  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.authService.decodeToken.nameid, photo.id).subscribe(() => {
      this.currentMain = _.findWhere(this.photos, { isMain: true });
      this.currentMain.isMain = false;
      photo.isMain = true;
      this.authService.changeMemberPhoto(photo.url);
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, error => {
      this.alertityService.error(error);
    });
  }


}
