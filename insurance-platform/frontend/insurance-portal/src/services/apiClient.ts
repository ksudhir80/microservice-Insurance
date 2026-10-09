import { getAccessToken } from "./authService";

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ||
  "http://localhost:5000";

export async function apiGet<T>(
  url: string
): Promise<T> {
  const token = getAccessToken();

  const response = await fetch(
    `${API_BASE_URL}${url}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...(token
          ? {
              Authorization: `Bearer ${token}`,
            }
          : {}),
      },
    }
  );

  if (!response.ok) {
    const errorText = await response.text();

    throw new Error(
      errorText ||
        `Request failed with status ${response.status}`
    );
  }

  return response.json();
}

export async function apiPost<T>(
  url: string,
  body: unknown
): Promise<T> {
  const token = getAccessToken();

  const response = await fetch(
    `${API_BASE_URL}${url}`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        ...(token
          ? {
              Authorization: `Bearer ${token}`,
            }
          : {}),
      },
      body: JSON.stringify(body),
    }
  );

  if (!response.ok) {
    const errorText = await response.text();

    throw new Error(
      errorText ||
        `Request failed with status ${response.status}`
    );
  }

  return response.json();
}