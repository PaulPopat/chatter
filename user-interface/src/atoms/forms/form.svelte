<script>
  import { createEventDispatcher } from "svelte";
  import create_url from "../../utils/url";
  import { SSO_URL } from "../../app/constants";

  const body_type = ["POST", "PUT", "PATCH"];

  const dispatch = createEventDispatcher();

  export let url;
  export let area = SSO_URL;
  export let method;

  function on_submit(e) {
    e.preventDefault();

    /** @type {HTMLFormElement} */
    const form = e.target;

    const data = new FormData(form);

    const input = {};
    for (const [key, value] of data) {
      input[key] = value;
    }

    const include_body = body_type.includes(method);
    const target = include_body
      ? create_url(area, url)
      : create_url(area, url, input);
    fetch(target, {
      method: method,
      body: include_body ? JSON.stringify(input) : undefined,
      headers: {
        "Content-Type": include_body ? "application/json" : undefined,
      },
    }).then((r) =>
      !r.ok
        ? dispatch("error", r)
        : r
            .json()
            .then((json) => dispatch("success", { data: json, response: r }))
    );
  }
</script>

<form on:submit={on_submit}>
  <slot />
</form>
