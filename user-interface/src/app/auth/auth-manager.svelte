<script>
  import { setContext } from "svelte";
  import TextInput from "../../atoms/forms/text-input.svelte";
  import Button from "../../atoms/button.svelte";
  import Form from "../../atoms/forms/form.svelte";

  let auth = {
    admin_token: "",
    server_token: "",
    user_id: "",
  };

  setContext("auth", {
    ...auth,
    get is_authenticated() {
      return !!auth.admin_token;
    },
  });

  function on_login(e) {
    auth = {
      admin_token: e.detail.data.AdminToken,
      server_token: e.detail.data.ServerToken,
      user_id: e.detail.data.UserId,
    };
  }
</script>

{#if !!auth.admin_token}
  <slot />
{:else}
  <div>
    <Form method="GET" url="/api/v1/auth/token" on:success={on_login}>
      <TextInput type="text" name="email">Email</TextInput>
      <TextInput type="password" name="password">Password</TextInput>
      <Button type="submit">Login</Button>
    </Form>
    <Form method="POST" url="/api/v1/users" on:success={on_login}>
      <TextInput type="text" name="UserName">User Name</TextInput>
      <TextInput type="text" name="Email">Email</TextInput>
      <TextInput type="password" name="Password">Password</TextInput>
      <Button type="submit">Register</Button>
    </Form>
  </div>
{/if}
