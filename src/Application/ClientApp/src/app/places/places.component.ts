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
  public cityName: string;
  public cityId: string;

  constructor(private route: ActivatedRoute,
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.cityId = this.route.snapshot.paramMap.get("cityId");
    this.cityName = this.route.snapshot.paramMap.get("cityName");
    http.get<Places[]>(baseUrl + 'api/places/GetAll/' + this.cityId).subscribe(result => {
      this.places = result;
      },
      error => console.error(error));
  }
}

interface Places {
  id: string;
  name: string;
  summary: string;
  imageUri: string;
}
