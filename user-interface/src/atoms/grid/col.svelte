<script>
  export let span = "3";
</script>

<div style={`grid-column: span ${span}`}>
  <slot />
</div>

<style>
  div {
    container-type: inline-size;
    container-name: col;

    display: block;
  }
</style>
