// Klasy użytkownika i DTO użytkownika, identyczne jak te w back-endzie

export class UserDetails {
    id: number = 0
    name: string = ''
    surname: string = ''
    email: string = ''
    password: string = ''
    phone: string = ''
    dateOfBirth: string = ''
    category: 'Służbowy' | 'Prywatny' | 'Inny' = 'Inny'
    subcategory: string | null = null
}
 
export class UserDetailsDTO {
    id: number = 0
    name: string = ''
    surname: string = ''
    email: string = ''
    phone: string = ''
    dateOfBirth: string = ''
    category: 'Służbowy' | 'Prywatny' | 'Inny' = 'Inny'
    subcategory: string | null = null
}