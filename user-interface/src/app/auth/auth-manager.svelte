<script>
  import { setContext } from "svelte";
  import TextInput from "../../atoms/forms/text-input.svelte";
  import Button from "../../atoms/button.svelte";

  let auth = {
    admin_token: "",
    server_token: "",
  };

  setContext("auth", {
    ...auth,
    get is_authenticated() {
      return !!auth.admin_token;
    },
  });

  /**
   * @param {Event} e
   */
  function on_submit(e) {
    const t = e.target;
    if (!(t instanceof HTMLFormElement)) return;
  }
</script>

{#if !!auth.admin_token}
  <slot />
{:else}
  <div>
    <form on:submit={on_submit}>
      <TextInput type="text" name="email">Email</TextInput>
      <TextInput type="password" name="password">Password</TextInput>
      <Button>Login</Button>
    </form>
  </div>
{/if}
