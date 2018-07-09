import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent {
  public locations: Locations[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Locations[]>(baseUrl + 'api/Locations/GetAll').subscribe(result => {
      this.locations = result;
    }, error => console.error(error));
  }
}

interface Locations {
  id: string;
  name: string;
  summary: string;
  imageUri: string;
}
