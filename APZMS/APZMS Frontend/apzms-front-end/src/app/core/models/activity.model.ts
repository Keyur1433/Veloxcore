export interface ActivityResponseDto {
    id: number
    name: string
    description: string
    price: number
    capacity: number
    minAge: number
    maxAge: number
    safetyLevel: string
    photoUrl: string
}

export interface AddActivityDto {
    name: string
    description: string
    price: number
    capacity: number
    minAge: number
    maxAge: number
    safetyLevel: string
    photo: File
}

export interface ActivityFilters {
    ageGroup?: string
    safetyLevel?: string
}