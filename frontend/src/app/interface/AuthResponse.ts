import { User } from "./User";

export interface AuthResponse {
  message: string;
  token?: string;
  user?: User;
}