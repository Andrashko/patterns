import { ref } from "vue";

export type GradeRead = {
  id: number;
  student_id: number;
  subject_id: number;
  student_name: string;
  subject_name: string;
  value: number;
  ects: string;
  five: number;
  grade_type: string;
  grade_date: string;
};

export type GradeCreate = {
  student_id: number;
  subject_id: number;
  value: number;
  grade_type: "current" | "module" | "exam" | "final";
  grade_date: string;
};

// Буде працювати і з proxy, і без нього:
// - якщо є proxy: API_BASE = "" і ходимо на "/api/..."
// - якщо proxy нема: можна поставити "http://127.0.0.1:8000"
const API_BASE = ""; // залишаємо порожнім для proxy

async function httpJson<T>(path: string, init?: RequestInit, timeoutMs = 10000): Promise<T> {
  const controller = new AbortController();
  const timer = setTimeout(() => controller.abort(), timeoutMs);

  try {
    const r = await fetch(`${API_BASE}${path}`, { ...init, signal: controller.signal });

    if (!r.ok) {
      const text = await r.text().catch(() => "");
      throw new Error(text || `HTTP ${r.status} ${r.statusText}`);
    }

    // DELETE може повертати {"ok": true}, це теж JSON
    return (await r.json()) as T;
  } catch (e: any) {
    if (e?.name === "AbortError") {
      throw new Error(`Timeout after ${timeoutMs}ms (запит завис або бекенд не відповідає)`);
    }
    throw e;
  } finally {
    clearTimeout(timer);
  }
}

export function useGradesVM() {
  const items = ref<GradeRead[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const lastRequest = ref<string | null>(null);

  async function load(): Promise<void> {
    loading.value = true;
    error.value = null;
    lastRequest.value = "GET /api/grades";
    try {
      items.value = await httpJson<GradeRead[]>("/api/grades");
    } catch (e: any) {
      error.value = String(e?.message ?? e);
    } finally {
      loading.value = false;
    }
  }

  async function create(payload: GradeCreate): Promise<void> {
    error.value = null;
    lastRequest.value = "POST /api/grades";
    const created = await httpJson<GradeRead>(
      "/api/grades",
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      }
    );
    items.value = [created, ...items.value];
  }

  async function remove(id: number): Promise<void> {
    error.value = null;
    lastRequest.value = `DELETE /api/grades/${id}`;
    await httpJson<{ ok: boolean }>(`/api/grades/${id}`, { method: "DELETE" });
    items.value = items.value.filter((x) => x.id !== id);
  }

  return { items, loading, error, lastRequest, load, create, remove };
}
