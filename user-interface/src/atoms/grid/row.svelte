<div>
  <slot />
</div>

<style>
  div {
    container-type: size;
    container-name: row;

    display: grid;
    gap: 0.5rem;
    grid-template-columns: repeat(3, minmax(0, 1fr));
  }

  @container (width > 600px) {
    div {
      grid-template-columns: repeat(6, minmax(0, 1fr));
    }
  }

  @container (width > 900px) {
    div {
      grid-template-columns: repeat(9, minmax(0, 1fr));
    }
  }

  @container (width > 1200px) {
    div {
      grid-template-columns: repeat(12, minmax(0, 1fr));
    }
  }
</style>
