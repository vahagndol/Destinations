// Shapes of the JSON the Application host passes through from Locations.API and Places.API
// (see src/Domain/Entities).

export interface Location {
  id: number
  name: string
  summary: string
  imageUri: string | null
}

export interface Place extends Location {
  cityId: number
}
