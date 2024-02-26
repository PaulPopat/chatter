<script>
  export let name;
  export let init = "";

  export let type = "text";
  export let mode = "text";
</script>

<label>
  <div>
    <slot />
  </div>

  <input {type} inputmode={mode} value={init} {name} />
</label>

<style>
  label {
    background: var(--colours-highlight-background);
    color: var(--colours-highlight-foreground);
    font-family: var(--font-family);
    font-size: var(--font-size-body);

    display: block;

    border-bottom: 1px solid var(--colours-highlight-foreground);

    --label-padding: 0.5rem;
    padding: var(--label-padding);

    transition: border-color 100ms;
  }

  label:focus-within {
    border-color: var(--colours-theme-secondary-background);
  }

  input {
    font-family: var(--font-family);
    font-size: var(--font-size-body);

    display: block;
    border: none;

    width: calc(100% - var(--label-padding));
    margin: 0;
    padding: 0.25rem;
    box-sizing: content-box;
  }

  input:focus {
    outline: none;
  }

  div {
    margin-bottom: var(--label-padding);
  }
</style>
