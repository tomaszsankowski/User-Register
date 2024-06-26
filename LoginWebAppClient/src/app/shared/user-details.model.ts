export class UserDetails {
    // Exact same class as the User class in the back-end User.cs
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
    // Exact same class as the User class in the back-end UserDTO.cs
    id: number = 0
    name: string = ''
    surname: string = ''
    email: string = ''
    phone: string = ''
    dateOfBirth: string = ''
    category: 'Służbowy' | 'Prywatny' | 'Inny' = 'Inny'
    subcategory: string | null = null
}