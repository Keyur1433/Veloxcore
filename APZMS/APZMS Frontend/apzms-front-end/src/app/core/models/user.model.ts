export interface LoginResponseDto {
    success: boolean;
    errorMessage: string | null;
    data: {
        customerId: number;
        customerName: string;
        ageGroup: string;
        accessToken: string;
        role: string;
    };
}

export interface User {
    id: string;
    name: string;
    role: string;
    token: string;
    expiresAt: number; // unix seconds
}

export interface UserRegisterDto {
    name: string
    email: string
    phone: string,
    password: string,
    birthDate: string
}