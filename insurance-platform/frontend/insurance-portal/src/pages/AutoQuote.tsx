import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import { apiPost } from "../services/apiClient";

type AutoQuoteForm = z.infer<typeof autoQuoteSchema>;

interface AutoQuoteResponse {
  quoteId: string;
  customerId: number;
  basePremium: number;
  riskAdjustment: number;
  coverageAdjustment: number;
  premium: number;
  monthlyPremium: number;
  status: string;
}

const autoQuoteSchema = z.object({
  make: z.string().min(1, "Vehicle make is required"),
  model: z.string().min(1, "Vehicle model is required"),

  year: z.coerce
    .number()
    .min(1900, "Enter a valid vehicle year")
    .max(new Date().getFullYear(), "Vehicle year cannot be in the future"),

  vin: z
    .string()
    .min(5, "VIN is required"),

  dateOfBirth: z
    .string()
    .min(1, "Date of birth is required")
    .refine(
      (date) => new Date(date) < new Date(),
      "Date of birth cannot be in the future"
    ),

  licenseState: z.string().min(1, "License state is required"),

  accidents: z.coerce
    .number()
    .min(0, "Accidents cannot be negative"),

  violations: z.coerce
    .number()
    .min(0, "Violations cannot be negative"),

  bodilyInjury: z.string().min(1, "Bodily injury coverage is required"),

  propertyDamage: z.coerce
    .number()
    .min(0, "Property damage cannot be negative"),

  collision: z.boolean(),

  comprehensive: z.boolean(),
});


export default function AutoQuote() {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<AutoQuoteForm>({
    resolver: zodResolver(autoQuoteSchema),
    defaultValues: {
      make: "Toyota",
      model: "Camry",
      year: 2024,
      vin: "1HGCM82633A123456",
      dateOfBirth: "1985-09-01",
      licenseState: "Pennsylvania",
      accidents: 0,
      violations: 0,
      bodilyInjury: "100/300",
      propertyDamage: 100000,
      collision: true,
      comprehensive: true,
    },
  });

 const onSubmit = async (data: AutoQuoteForm) => {
  try {
    // Save the completed form locally
    localStorage.setItem(
      "autoQuoteForm",
      JSON.stringify(data)
    );

    const requestBody = {
      customerId: 1,

      vehicle: {
        make: data.make,
        model: data.model,
        year: data.year,
        vin: data.vin,
      },

      driver: {
        dateOfBirth: data.dateOfBirth,
        licenseState: data.licenseState,
        accidents: data.accidents,
        violations: data.violations,
      },

      coverage: {
        bodilyInjury: data.bodilyInjury,
        propertyDamage: data.propertyDamage,
        collision: data.collision,
        comprehensive: data.comprehensive,
      },
    };

    console.log(
      "Submitting Auto Quote:",
      requestBody
    );

    const quote = await apiPost<AutoQuoteResponse>(
      "/api/quotes/auto",
      requestBody
    );

    console.log("Quote Response:", quote);

    // Save quote result
    localStorage.setItem(
      "autoQuoteResult",
      JSON.stringify(quote)
    );

    navigate("/auto-quote/result", {
      state: quote,
    });
  } catch (error) {
    console.error(
      "Quote creation failed:",
      error
    );

    alert(
      error instanceof Error
        ? error.message
        : "Unable to create quote."
    );
  }
};

  return (
    <div style={{ maxWidth: "900px", margin: "0 auto", padding: "20px" }}>
      <button
        type="button"
        onClick={() => navigate("/")}
        style={{
          padding: "10px 20px",
          marginBottom: "30px",
          cursor: "pointer",
        }}
      >
        ← Dashboard
      </button>

      <h1 style={{ textAlign: "center", fontSize: "52px" }}>
        Auto Insurance Quote
      </h1>

      <p
        style={{
          textAlign: "center",
          fontSize: "25px",
          color: "#666",
        }}
      >
        Get your personalized auto insurance quote
      </p>

      <form onSubmit={handleSubmit(onSubmit)}>

        {/* VEHICLE */}

        <div
          style={{
            border: "1px solid #ddd",
            borderRadius: "12px",
            padding: "30px",
            marginTop: "30px",
          }}
        >
          <h2>🚗 Vehicle Information</h2>

          <div className="form-row">
            <div>
              <label>Make</label>
              <input {...register("make")} />
              {errors.make && (
                <p className="error">{errors.make.message}</p>
              )}
            </div>

            <div>
              <label>Model</label>
              <input {...register("model")} />
              {errors.model && (
                <p className="error">{errors.model.message}</p>
              )}
            </div>
          </div>

          <div className="form-row">
            <div>
              <label>Year</label>
              <input
                type="number"
                {...register("year")}
              />
              {errors.year && (
                <p className="error">{errors.year.message}</p>
              )}
            </div>

            <div>
              <label>VIN</label>
              <input {...register("vin")} />
              {errors.vin && (
                <p className="error">{errors.vin.message}</p>
              )}
            </div>
          </div>
        </div>

        {/* DRIVER */}

        <div
          style={{
            border: "1px solid #ddd",
            borderRadius: "12px",
            padding: "30px",
            marginTop: "30px",
          }}
        >
          <h2>👤 Driver Information</h2>

          <div className="form-row">
            <div>
              <label>Date of Birth</label>
              <input
                type="date"
                {...register("dateOfBirth")}
              />

              {errors.dateOfBirth && (
                <p className="error">
                  {errors.dateOfBirth.message}
                </p>
              )}
            </div>

            <div>
              <label>License State</label>

              <select {...register("licenseState")}>
                <option value="Pennsylvania">
                  Pennsylvania
                </option>
                <option value="Ohio">Ohio</option>
                <option value="New York">
                  New York
                </option>
                <option value="New Jersey">
                  New Jersey
                </option>
                <option value="California">
                  California
                </option>
                <option value="Texas">Texas</option>
              </select>

              {errors.licenseState && (
                <p className="error">
                  {errors.licenseState.message}
                </p>
              )}
            </div>
          </div>

          <div className="form-row">
            <div>
              <label>Accidents</label>
              <input
                type="number"
                min="0"
                {...register("accidents")}
              />

              {errors.accidents && (
                <p className="error">
                  {errors.accidents.message}
                </p>
              )}
            </div>

            <div>
              <label>Violations</label>
              <input
                type="number"
                min="0"
                {...register("violations")}
              />

              {errors.violations && (
                <p className="error">
                  {errors.violations.message}
                </p>
              )}
            </div>
          </div>
        </div>

        {/* COVERAGE */}

        <div
          style={{
            border: "1px solid #ddd",
            borderRadius: "12px",
            padding: "30px",
            marginTop: "30px",
          }}
        >
          <h2>🛡️ Coverage Information</h2>

          <div className="form-row">
            <div>
              <label>Bodily Injury</label>

              <select {...register("bodilyInjury")}>
                <option value="25/50">25/50</option>
                <option value="50/100">50/100</option>
                <option value="100/300">100/300</option>
              </select>
            </div>

            <div>
              <label>Property Damage</label>

              <select
                {...register("propertyDamage")}
              >
                <option value="50000">$50,000</option>
                <option value="100000">$100,000</option>
                <option value="250000">$250,000</option>
              </select>
            </div>
          </div>

          <div
            style={{
              marginTop: "20px",
              display: "flex",
              gap: "30px",
            }}
          >
            <label>
              <input
                type="checkbox"
                {...register("collision")}
              />
              Collision
            </label>

            <label>
              <input
                type="checkbox"
                {...register("comprehensive")}
              />
              Comprehensive
            </label>
          </div>
        </div>

        {/* SUBMIT */}

        <div
          style={{
            textAlign: "center",
            marginTop: "35px",
            marginBottom: "50px",
          }}
        >
          <button
            type="submit"
            disabled={isSubmitting}
            style={{
              padding: "15px 50px",
              fontSize: "20px",
              fontWeight: "bold",
              cursor: "pointer",
            }}
          >
            {isSubmitting
              ? "Getting Quote..."
              : "Get My Quote"}
          </button>
        </div>
      </form>

      <style>{`
        .form-row {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 30px;
          margin-top: 20px;
        }

        label {
          display: block;
          font-size: 18px;
          margin-bottom: 8px;
        }

        input,
        select {
          width: 100%;
          box-sizing: border-box;
          padding: 10px;
          font-size: 16px;
        }

        input[type="checkbox"] {
          width: auto;
          margin-right: 8px;
        }

        .error {
          color: red;
          font-size: 14px;
          margin-top: 5px;
        }

        @media (max-width: 700px) {
          .form-row {
            grid-template-columns: 1fr;
          }

          h1 {
            font-size: 36px !important;
          }
        }
      `}</style>
    </div>
  );
}