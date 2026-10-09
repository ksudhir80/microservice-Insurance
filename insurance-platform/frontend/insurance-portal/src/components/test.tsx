import React, { useEffect, useState } from "react";

interface Customer {
  id: string;
  name: string;
  email: string;
}

function useDebounce<T>(value: T, delay: number): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(timer);
    };
  }, [value, delay]);

  return debouncedValue;
}

function Test() {
  const [search, setSearch] = useState("");

  // Wait 500ms after the user stops typing
  const debouncedSearch = useDebounce(search, 500);

  const [data, setData] = useState<Customer[]>([]);
  const [loading, setLoading] = useState(false);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    if (debouncedSearch.length <= 5) {
      setData([]);
      return;
    }

    const searchCustomers = async () => {
      try {
        setLoading(true);
        setIsError(false);

        const response = await fetch(
          `/api/customers/search?search=${encodeURIComponent(
            debouncedSearch
          )}`
        );

        if (!response.ok) {
          throw new Error("Failed to search customers");
        }

        const result: Customer[] = await response.json();

        setData(result);
      } catch (error) {
        console.error(error);
        setIsError(true);
        setData([]);
      } finally {
        setLoading(false);
      }
    };

    searchCustomers();
  }, [debouncedSearch]);

  return (
    <div>
      <input
        type="text"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search customer..."
      />

      {loading && <p>Searching...</p>}

      {isError && <p>Something went wrong.</p>}

      {!loading && !isError && data.length > 0 && (
        <ul>
          {data.map((customer) => (
            <li key={customer.id}>
              {customer.name} - {customer.email}
            </li>
          ))}
        </ul>
      )}

      {!loading &&
        !isError &&
        debouncedSearch.length > 5 &&
        data.length === 0 && <p>No customers found.</p>}
    </div>
  );
}

export default Test;