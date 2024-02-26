<script>
  export let type = "button";
  export let colour = "primary";
</script>

<button {type} class={`cl-${colour}`}>
  <slot />
</button>

<style>
  button {
    font-family: var(--font-family);
    font-size: var(--font-size-body);

    border: none;
    box-shadow: none;
    padding: 0.5rem;

    border-radius: 3px;
    cursor: pointer;
    transition: box-shadow 50ms;
    margin: 0;
  }

  button:hover,
  button:focus {
    box-shadow: inset 0 0 10px 10px rgba(255, 255, 255, 0.4);
  }

  .cl-body {
    background: var(--colours-body-background);
    color: var(--colours-body-foreground);
  }

  .cl-highlight {
    background: var(--colours-highlight-background);
    color: var(--colours-highlight-foreground);
  }

  .cl-primary {
    background: var(--colours-theme-primary-background);
    color: var(--colours-theme-primary-foreground);
  }

  .cl-secondary {
    background: var(--colours-theme-secondary-background);
    color: var(--colours-theme-secondary-foreground);
  }

  .cl-warning {
    background: var(--colours-theme-warning-background);
    color: var(--colours-theme-warning-foreground);
  }

  .cl-error {
    background: var(--colours-theme-error-background);
    color: var(--colours-theme-error-foreground);
  }
</style>
