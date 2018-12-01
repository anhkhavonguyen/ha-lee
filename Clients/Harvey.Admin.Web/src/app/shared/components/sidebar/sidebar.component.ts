import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { ChangePasswordComponent } from 'src/app/shared/components/change-password/change-password.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
    isActive = false;
    showMenu = '';
    pushRightClass = 'push-right';
    @Input()
    userName = '';
    constructor(
        private authService: AuthService,
        private modalService: NgbModal) { }

    ngOnInit() {
    }
    eventCalled() {
        this.isActive = !this.isActive;
    }

    addExpandClass(element: any) {
        if (element === this.showMenu) {
            this.showMenu = '0';
        } else {
            this.showMenu = element;
        }
    }

    isToggled(): boolean {
        const dom = document.querySelector('body');
        return dom ? dom.classList.contains(this.pushRightClass) : false;
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    onLoggedout() {
        this.authService.logout();
    }

    onChangePassword() {
        this.modalService.open(ChangePasswordComponent, { size: 'lg', centered: true, backdrop: 'static' });
      }

}
