import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css']
})
export class PlacesComponent {
  public places: Places[];
  public locationName: string;
  public locationId: string;

  constructor(private route: ActivatedRoute,
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.locationId = this.route.snapshot.paramMap.get("locationId");
    this.locationName = this.route.snapshot.paramMap.get("locationName");
    http.get<Places[]>(baseUrl + 'api/places/GetAll/' + this.locationId).subscribe(result => {
      this.places = result;
      },
      error => console.error(error));
  }
}

interface Places {
  cityId: string,
  id: string;
  name: string;
  summary: string;
  imageUri: string;
}
