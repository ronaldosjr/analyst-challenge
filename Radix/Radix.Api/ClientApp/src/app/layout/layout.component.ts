import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

interface NavLink {
  name: string;
  link: string;
}

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  title = 'Radix';

  navItems: NavLink[] = [
    { name: 'Tabela', link: 'table' },
    { name: 'Gráfico', link: 'chart' },
  ];

  constructor(private breakpointObserver: BreakpointObserver) {}

}
