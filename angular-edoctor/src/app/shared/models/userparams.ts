import { User } from './user';

export class UserParams {
    specialtyId = 0;
    officeId = 0;
    query: string;
    status: string;
    page = 1;
    pageCount = 10;
    sort = 'city';
    username: string;

    constructor(user: User) {
         this.username = user.username;
    }
}
