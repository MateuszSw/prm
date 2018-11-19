import { ViewContainerRef, TemplateRef, Directive, Input, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  isVisible = false;
  constructor(
    private viewContainer: ViewContainerRef,
    private template: TemplateRef<any>,
    private auth: AuthService) { }
  ngOnInit() {
    const userRoles = this.auth.token.role as Array<string>;
    // jeśli żadna rola czyść widok
    if (!userRoles) {
      this.viewContainer.clear();
    }
    // jeśli użytkownik ma odpowiednią role wyświetl opcje
    if (this.auth.role(this.appHasRole)) {
      if (!this.isVisible) {
        this.isVisible = true;
        this.viewContainer.createEmbeddedView(this.template);
      } else {
        this.isVisible = false;
        this.viewContainer.clear();
 }}}

}
