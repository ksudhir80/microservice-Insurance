const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5000";

interface LoginResponse {
  accessToken: string;
  tokenType: string;
  role: string;
  expiresIn: number;
}

export async function login(
  username: string,
  password: string
): Promise<LoginResponse> {
  const response = await fetch(`${API_BASE_URL}/auth/token`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      username,
      password,
    }),
  });

  if (!response.ok) {
    throw new Error("Invalid username or password.");
  }

  const data: LoginResponse = await response.json();

  localStorage.setItem("accessToken", data.accessToken);
  localStorage.setItem("userRole", data.role);
  localStorage.setItem("username", username);

  return data;
}

export function getAccessToken(): string | null {
  return localStorage.getItem("accessToken");
}

export function getUserRole(): string | null {
  return localStorage.getItem("userRole");
}

export function getUsername(): string | null {
  return localStorage.getItem("username");
}

export function isAuthenticated(): boolean {
  return !!getAccessToken();
}

export function logout(): void {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("userRole");
  localStorage.removeItem("username");
}