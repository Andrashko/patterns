<script setup lang="ts">
import { computed, onMounted, reactive, ref } from "vue";
import { useGradesVM, type GradeCreate } from "../viewmodels/grades.vm";

const vm = useGradesVM();

const form = reactive<GradeCreate>({
  student_id: 1,
  subject_id: 1,
  value: 80,
  grade_type: "exam",
  grade_date: new Date().toISOString().slice(0, 10),
});

const busy = ref(false);

// Нормалізація items: працює і якщо items — Ref, і якщо items — звичайний масив
const items = computed<any[]>(() => {
  const raw: any = (vm as any).items;
  const v = raw?.value ?? raw; // якщо Ref -> value, якщо масив -> сам raw
  return Array.isArray(v) ? v : [];
});

const lastRequest = computed<string>(() => String((vm as any).lastRequest?.value ?? (vm as any).lastRequest ?? "—"));
const loading = computed<boolean>(() => Boolean((vm as any).loading?.value ?? (vm as any).loading ?? false));
const error = computed<string>(() => String((vm as any).error?.value ?? (vm as any).error ?? ""));

onMounted(async () => {
  await (vm as any).load?.();
});

async function submit() {
  busy.value = true;
  try {
    await (vm as any).create?.({ ...form });
  } catch (e: any) {
    alert(String(e?.message ?? e));
  } finally {
    busy.value = false;
  }
}

async function del(id: number) {
  if (!confirm("Вилучити оцінку?")) return;
  try {
    await (vm as any).remove?.(id);
  } catch (e: any) {
    alert(String(e?.message ?? e));
  }
}
</script>

<template>
  <div style="max-width: 1100px; margin: 0 auto; padding: 16px; font-family: system-ui;">
    <h1>Grades (MVVM)</h1>

    <div style="margin-bottom: 8px;">
      <button @click="(vm as any).load?.()" :disabled="loading">Reload</button>
      <span v-if="loading" style="margin-left: 8px;">Loading...</span>
      <span v-if="error" style="margin-left: 8px; color:#b00020;">{{ error }}</span>
    </div>

    <!-- <div style="color:#666; margin-bottom: 10px;">
      last request: {{ lastRequest }} · items: {{ items.length }}
    </div>

    DEBUG (потім можна прибрати)
    <details style="margin: 10px 0;">
      <summary>debug vm.items</summary>
      <pre style="white-space: pre-wrap;">{{ (vm as any).items }}</pre>
    </details> -->

    <h2>Add grade</h2>
    <div style="display:flex; gap:8px; flex-wrap:wrap; align-items:center; margin-bottom: 12px;">
      <input type="number" v-model.number="form.student_id" min="1" placeholder="Student ID" />
      <input type="number" v-model.number="form.subject_id" min="1" placeholder="Subject ID" />
      <input type="number" v-model.number="form.value" min="0" max="100" placeholder="Value" />
      <select v-model="form.grade_type">
        <option value="current">current</option>
        <option value="module">module</option>
        <option value="exam">exam</option>
        <option value="final">final</option>
      </select>
      <input type="date" v-model="form.grade_date" />
      <button @click="submit" :disabled="busy">Save</button>
    </div>

    <h2>Grades</h2>
    <table border="1" cellpadding="6" cellspacing="0" style="width:100%; border-collapse:collapse;">
      <thead>
        <tr>
          <th>ID</th><th>Date</th><th>Student</th><th>Subject</th><th>Type</th>
          <th>100</th><th>ECTS</th><th>5</th><th></th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="g in items" :key="g.id">
          <td>{{ g.id }}</td>
          <td>{{ g.grade_date }}</td>
          <td>{{ g.student_name }}</td>
          <td>{{ g.subject_name }}</td>
          <td>{{ g.grade_type }}</td>
          <td style="text-align:center;">{{ g.value }}</td>
          <td style="text-align:center;">{{ g.ects }}</td>
          <td style="text-align:center;">{{ g.five }}</td>
          <td style="text-align:center;"><button @click="del(g.id)">Delete</button></td>
        </tr>

        <tr v-if="items.length === 0">
          <td colspan="9" style="text-align:center; color:#666;">No data</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
