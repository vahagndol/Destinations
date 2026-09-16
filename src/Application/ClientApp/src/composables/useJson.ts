import { shallowRef, toValue, watchEffect, type MaybeRefOrGetter } from 'vue'

/**
 * Fetches JSON from `url` and re-fetches whenever a reactive `url` changes,
 * aborting the previous request.
 */
export function useJson<T>(url: MaybeRefOrGetter<string>) {
  const data = shallowRef<T>()
  const error = shallowRef<string>()

  watchEffect((onCleanup) => {
    const controller = new AbortController()
    onCleanup(() => controller.abort())

    data.value = undefined
    error.value = undefined

    fetch(toValue(url), { signal: controller.signal })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`${response.status} ${response.statusText}`.trim())
        }
        return response.json() as Promise<T>
      })
      .then((result) => {
        data.value = result
      })
      .catch((e: unknown) => {
        if (controller.signal.aborted) return
        console.error(e)
        error.value = e instanceof Error ? e.message : String(e)
      })
  })

  return { data, error }
}
